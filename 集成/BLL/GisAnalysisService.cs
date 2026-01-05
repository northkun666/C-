// BLL/GisAnalysisService.cs
using System;
using System.Collections.Generic;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Topology;
using 集成.Models; // 引用 Model

namespace 集成.BLL
{
    public class GisAnalysisService
    {
        // 1. 提取高程逻辑 (从 Form1 移出)
        // 这是一个私有辅助方法
        private List<PathPoint> ExtractElevationSegment(double startX, double startY, double endX, double endY, IMapRasterLayer raster)
        {
            double curX = startX;
            double curY = startY;
            double curElevation = 0;
            List<PathPoint> pathPointList = new List<PathPoint>();
            int numberofpoints = 100;
            double constXdif = ((endX - startX) / numberofpoints);
            double constYdif = ((endY - startY) / numberofpoints);

            for (int i = 0; i <= numberofpoints; i++)
            {
                PathPoint newPathPoint = new PathPoint();
                if ((i == 0)) { curX = startX; curY = startY; }
                else { curX += constXdif; curY += constYdif; }

                Coordinate coordinate = new Coordinate(curX, curY);

                // 边界检查逻辑 (之前 Debug 过的)
                if (raster.DataSet.Bounds != null)
                {
                    RcIndex rowColumn = raster.DataSet.Bounds.ProjToCell(coordinate);
                    if (rowColumn.Row >= 0 && rowColumn.Row < raster.DataSet.NumRows &&
                        rowColumn.Column >= 0 && rowColumn.Column < raster.DataSet.NumColumns)
                    {
                        curElevation = raster.DataSet.Value[rowColumn.Row, rowColumn.Column];
                    }
                    else { curElevation = 0; }
                }

                newPathPoint.X = curX;
                newPathPoint.Y = curY;
                newPathPoint.Elevation = curElevation;
                pathPointList.Add(newPathPoint);
            }
            return pathPointList;
        }

        // 2. 计算完整徒步路径 (这是 UI 调用的主方法)
        public List<PathPoint> CalculateHikingProfile(IFeature pathFeature, IMapRasterLayer demLayer)
        {
            IList<Coordinate> coordinateList = pathFeature.Coordinates;
            List<PathPoint> fullPathList = new List<PathPoint>();

            // 提取点
            for (int i = 0; i < coordinateList.Count - 1; i++)
            {
                Coordinate start = coordinateList[i];
                Coordinate end = coordinateList[i + 1];
                var segment = ExtractElevationSegment(start.X, start.Y, end.X, end.Y, demLayer);
                fullPathList.AddRange(segment);
            }

            // 计算累计距离
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

        // 3. 栅格乘法逻辑
        public IRaster MultiplyRaster(IRaster sourceRaster, double multiplier, string outFileName)
        {
            // 这里可以调用 DAL 来创建文件，或者直接处理
            // 为简单起见，逻辑放在这里，文件创建放在这
            IRaster newRaster = Raster.CreateRaster(outFileName, null, sourceRaster.NumColumns, sourceRaster.NumRows, 1, sourceRaster.DataType, new string[0]);
            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

            for (int r = 0; r < sourceRaster.NumRows; r++)
            {
                for (int c = 0; c < sourceRaster.NumColumns; c++)
                {
                    if (sourceRaster.Value[r, c] != sourceRaster.NoDataValue)
                        newRaster.Value[r, c] = sourceRaster.Value[r, c] * multiplier;
                    else
                        newRaster.Value[r, c] = newRaster.NoDataValue;
                }
            }
            return newRaster;
        }

        // 4. 栅格重分类逻辑
        public IRaster ReclassifyRaster(IRaster sourceRaster, double threshold, string outFileName)
        {
            IRaster newRaster = Raster.CreateRaster(outFileName, null, sourceRaster.NumColumns, sourceRaster.NumRows, 1, sourceRaster.DataType, new string[0]);
            // ... 设置 Bounds, Projection ... (同上)
            newRaster.Bounds = sourceRaster.Bounds.Copy();
            newRaster.NoDataValue = sourceRaster.NoDataValue;
            newRaster.Projection = sourceRaster.Projection;

            for (int r = 0; r < sourceRaster.NumRows; r++)
            {
                for (int c = 0; c < sourceRaster.NumColumns; c++)
                {
                    if (sourceRaster.Value[r, c] != sourceRaster.NoDataValue)
                        newRaster.Value[r, c] = (sourceRaster.Value[r, c] >= threshold) ? 1 : 0;
                    else
                        newRaster.Value[r, c] = newRaster.NoDataValue;
                }
            }
            return newRaster;
        }
    }
}