using System;
using System.Collections.Generic;
using System.Linq; 
using DotSpatial.Data;     
using DotSpatial.Topology; 
using 集成.Models;         
using 集成.DAL;            
using DotSpatial.Symbology;
using System.Drawing;
namespace 集成.BLL
{
    public class GisAnalysisService
    {
        private FileRepository _fileRepo = new FileRepository();

        public void SaveShapefile(IFeatureSet fs, string filePath)
        {
            if (fs == null || fs.Features.Count == 0)
                throw new Exception("没有可保存的要素。");

            _fileRepo.SaveShapefile(fs, filePath);
        }

        public void SaveRaster(IRaster raster)
        {
            _fileRepo.SaveRaster(raster);
        }

        public double CalculateTotalArea(IFeatureSet featureSet)
        {
            double totalArea = 0;
            if (featureSet != null)
            {
                foreach (IFeature feature in featureSet.Features)
                {
                    totalArea += feature.Area();
                }
            }
            return totalArea;
        }

        public ColorScheme GetDynamicElevationScheme(IRaster raster)
        {
            if (raster == null) return new ColorScheme();

            if (raster.Minimum == 0 && raster.Maximum == 0 && raster.Value[0, 0] == 0)
            {
                raster.GetStatistics(); 
            }

            double min = raster.Minimum;
            double max = raster.Maximum;
            double mid = (min + max) / 2;

            ColorScheme scheme = new ColorScheme();

            var highCat = new ColorCategory(mid, max, Color.Orange, Color.Red);
            highCat.LegendText = string.Format("High ({0:F0} - {1:F0})", mid, max);

            var lowCat = new ColorCategory(min, mid, Color.Blue, Color.Green);
            lowCat.LegendText = string.Format("Low ({0:F0} - {1:F0})", min, mid);

            scheme.AddCategory(highCat);
            scheme.AddCategory(lowCat);

            return scheme;
        }

        public double CalculateRegionArea(IFeatureSet featureSet, string fieldName, string value)
        {
            double area = 0;
            if (featureSet != null)
            {
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

        private List<PathPoint> ExtractElevationSegment(double startX, double startY, double endX, double endY, IRaster raster)
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

                if (raster.Bounds != null)
                {
                    RcIndex rowColumn = raster.Bounds.ProjToCell(coordinate);

                    if (rowColumn.Row >= 0 && rowColumn.Row < raster.NumRows &&
                        rowColumn.Column >= 0 && rowColumn.Column < raster.NumColumns)
                    {
                        curElevation = raster.Value[rowColumn.Row, rowColumn.Column];
                    }
                    else
                    {
                        curElevation = 0; 
                    }
                }

                newPathPoint.X = curX;
                newPathPoint.Y = curY;
                newPathPoint.Elevation = curElevation;
                pathPointList.Add(newPathPoint);
            }
            return pathPointList;
        }
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
    }
}