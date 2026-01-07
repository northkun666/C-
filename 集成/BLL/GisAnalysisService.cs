using System;
using System.Collections.Generic;
using System.Linq; // 用于 SelectIndexByAttribute 等查询
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
            // 这里可以添加业务验证逻辑
            if (fs == null || fs.Features.Count == 0)
                throw new Exception("没有可保存的要素。");

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

        #region 面积计算逻辑 (从 FormProjection 迁移过来)

        /// <summary>
        /// 计算矢量图层中所有要素的总面积
        /// </summary>
        public double CalculateTotalArea(IFeatureSet featureSet)
        {
            double totalArea = 0;
            if (featureSet != null)
            {
                foreach (IFeature feature in featureSet.Features)
                {
                    // 注意：面积单位取决于投影坐标系
                    totalArea += feature.Area();
                }
            }
            return totalArea;
        }

        /// <summary>
        /// 根据属性查询计算特定区域的面积
        /// </summary>
        public double CalculateRegionArea(IFeatureSet featureSet, string fieldName, string value)
        {
            double area = 0;
            if (featureSet != null)
            {
                // 使用 SelectByAttribute 获取符合条件的要素索引
                // 注意：为了不影响 UI 的选中状态，我们只获取索引进行计算，或者需要 UI 层配合清除选中
                // 这里使用纯数据逻辑：遍历查找

                // 方法A: 遍历所有要素 (性能较低但稳定)
                foreach (IFeature feature in featureSet.Features)
                {
                    if (feature.DataRow[fieldName].ToString() == value)
                    {
                        area += feature.Area();
                    }
                }
            }
            return area;
        }

        #endregion

        #region 徒步路径计算逻辑

        // 辅助方法：从两点线段中按步长提取高程点
        private List<PathPoint> ExtractElevationSegment(double startX, double startY, double endX, double endY, IRaster raster)
        {
            double curX = startX;
            double curY = startY;
            double curElevation = 0;
            List<PathPoint> pathPointList = new List<PathPoint>();
            int numberofpoints = 100; // 采样点数量
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
        public List<PathPoint> CalculateHikingProfile(IFeature pathFeature, IRaster demRaster)
        {
            // 建议添加投影一致性检查
            if (pathFeature == null || demRaster == null)
                throw new Exception("路径或高程数据为空");

            IList<Coordinate> coordinateList = pathFeature.Coordinates;
            List<PathPoint> fullPathList = new List<PathPoint>();

            // 1. 遍历线段提取高程
            for (int i = 0; i < coordinateList.Count - 1; i++)
            {
                Coordinate start = coordinateList[i];
                Coordinate end = coordinateList[i + 1];
                var segment = ExtractElevationSegment(start.X, start.Y, end.X, end.Y, demRaster);
                fullPathList.AddRange(segment);
            }

            // 2. 计算累计距离
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
        /// 栅格乘法
        /// </summary>
        public IRaster MultiplyRaster(IRaster sourceRaster, double multiplier, string outFileName)
        {
            IRaster newRaster = _fileRepo.CreateResultRaster(outFileName, sourceRaster.NumColumns, sourceRaster.NumRows, sourceRaster.DataType, new string[0]);

            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

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
        /// 栅格重分类
        /// </summary>
        public IRaster ReclassifyRaster(IRaster sourceRaster, double threshold, string outFileName)
        {
            IRaster newRaster = _fileRepo.CreateResultRaster(outFileName, sourceRaster.NumColumns, sourceRaster.NumRows, sourceRaster.DataType, new string[0]);

            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

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