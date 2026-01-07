using System;
using System.Collections.Generic;
using DotSpatial.Data;     // 核心数据接口 (IRaster, IFeatureSet 等)
using DotSpatial.Topology; // 几何类型 (Coordinate, Point 等)
using 集成.Models;         // 引入 PathPoint 模型
using 集成.DAL;            // 引入数据访问层

namespace 集成.BLL
{
    public class GisAnalysisService
    {
        // 实例化 DAL 对象，处理底层文件操作
        private FileRepository _fileRepo = new FileRepository();

        #region 数据保存方法 (UI -> BLL -> DAL)

        /// <summary>
        /// 保存矢量文件 (Shapefile)
        /// </summary>
        public void SaveShapefile(IFeatureSet fs, string filePath)
        {
            // 这里可以添加业务验证逻辑，例如检查属性表完整性等
            _fileRepo.SaveShapefile(fs, filePath);
        }

        /// <summary>
        /// 保存栅格文件
        /// </summary>
        public void SaveRaster(IRaster raster)
        {
            _fileRepo.SaveRaster(raster);
        }

        #endregion

        #region 徒步路径计算逻辑

        // 辅助方法：从两点线段中按步长提取高程点
        // ★★★ 修正点：参数改为 IRaster，与 UI 控件解耦 ★★★
        private List<PathPoint> ExtractElevationSegment(double startX, double startY, double endX, double endY, IRaster raster)
        {
            double curX = startX;
            double curY = startY;
            double curElevation = 0;
            List<PathPoint> pathPointList = new List<PathPoint>();
            int numberofpoints = 100; // 采样点数量，可根据需要调整
            double constXdif = ((endX - startX) / numberofpoints);
            double constYdif = ((endY - startY) / numberofpoints);

            for (int i = 0; i <= numberofpoints; i++)
            {
                PathPoint newPathPoint = new PathPoint();
                if ((i == 0)) { curX = startX; curY = startY; }
                else { curX += constXdif; curY += constYdif; }

                Coordinate coordinate = new Coordinate(curX, curY);

                // 使用 raster.Bounds 进行坐标转换
                if (raster.Bounds != null)
                {
                    RcIndex rowColumn = raster.Bounds.ProjToCell(coordinate);

                    // 边界检查
                    if (rowColumn.Row >= 0 && rowColumn.Row < raster.NumRows &&
                        rowColumn.Column >= 0 && rowColumn.Column < raster.NumColumns)
                    {
                        curElevation = raster.Value[rowColumn.Row, rowColumn.Column];
                    }
                    else
                    {
                        curElevation = 0; // 或者设为 NoDataValue
                    }
                }

                newPathPoint.X = curX;
                newPathPoint.Y = curY;
                newPathPoint.Elevation = curElevation;
                pathPointList.Add(newPathPoint);
            }
            return pathPointList;
        }

        /// <summary>
        /// 计算完整的徒步路径剖面
        /// </summary>
        /// <param name="pathFeature">路径要素 (Line)</param>
        /// <param name="demRaster">高程数据 (IRaster)</param>
        /// <returns>包含距离和高程的点列表</returns>
        public List<PathPoint> CalculateHikingProfile(IFeature pathFeature, IRaster demRaster)
        {
            IList<Coordinate> coordinateList = pathFeature.Coordinates;
            List<PathPoint> fullPathList = new List<PathPoint>();

            // 1. 遍历线段提取高程
            for (int i = 0; i < coordinateList.Count - 1; i++)
            {
                Coordinate start = coordinateList[i];
                Coordinate end = coordinateList[i + 1];
                // 调用辅助方法
                var segment = ExtractElevationSegment(start.X, start.Y, end.X, end.Y, demRaster);
                fullPathList.AddRange(segment);
            }

            // 2. 计算累计距离 (用于 X 轴显示)
            double distanceFromStart = 0;
            for (int i = 1; i < fullPathList.Count; i++)
            {
                double x1 = fullPathList[i - 1].X;
                double y1 = fullPathList[i - 1].Y;
                double x2 = fullPathList[i].X;
                double y2 = fullPathList[i].Y;

                double dist = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                distanceFromStart += dist;
                fullPathList[i].Distance = distanceFromStart;
            }

            return fullPathList;
        }

        #endregion

        #region 栅格运算逻辑

        /// <summary>
        /// 栅格乘法 (每个像素值 * 倍数)
        /// </summary>
        public IRaster MultiplyRaster(IRaster sourceRaster, double multiplier, string outFileName)
        {
            // 调用 DAL 创建结果文件结构
            IRaster newRaster = _fileRepo.CreateResultRaster(outFileName, sourceRaster.NumColumns, sourceRaster.NumRows, sourceRaster.DataType, new string[0]);

            // 复制元数据
            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

            // 执行计算
            for (int r = 0; r < sourceRaster.NumRows; r++)
            {
                for (int c = 0; c < sourceRaster.NumColumns; c++)
                {
                    double val = sourceRaster.Value[r, c];
                    if (val != sourceRaster.NoDataValue)
                        newRaster.Value[r, c] = val * multiplier;
                    else
                        newRaster.Value[r, c] = newRaster.NoDataValue;
                }
            }
            return newRaster;
        }

        /// <summary>
        /// 栅格重分类 (大于阈值设为1，否则设为0)
        /// </summary>
        public IRaster ReclassifyRaster(IRaster sourceRaster, double threshold, string outFileName)
        {
            // 调用 DAL 创建结果文件结构
            IRaster newRaster = _fileRepo.CreateResultRaster(outFileName, sourceRaster.NumColumns, sourceRaster.NumRows, sourceRaster.DataType, new string[0]);

            // 复制元数据
            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

            // 执行计算
            for (int r = 0; r < sourceRaster.NumRows; r++)
            {
                for (int c = 0; c < sourceRaster.NumColumns; c++)
                {
                    double val = sourceRaster.Value[r, c];
                    if (val != sourceRaster.NoDataValue)
                        newRaster.Value[r, c] = (val >= threshold) ? 1 : 0;
                    else
                        newRaster.Value[r, c] = newRaster.NoDataValue;
                }
            }
            return newRaster;
        }

        #endregion
    }
}