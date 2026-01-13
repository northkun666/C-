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
            if (pathFeature == null || demRaster == null)
                throw new Exception("路径或高程数据为空");

            IList<Coordinate> coordinateList = pathFeature.Coordinates;
            List<PathPoint> fullPathList = new List<PathPoint>();

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
        public IRaster CalculateMeanFilter(IRaster sourceRaster, string outFileName)
        {
            // 创建结果栅格
            IRaster outRaster = _fileRepo.CreateResultRaster(outFileName, sourceRaster.NumColumns, sourceRaster.NumRows, sourceRaster.DataType, new string[0]);
            outRaster.Bounds = sourceRaster.Bounds.Copy();
            outRaster.Projection = sourceRaster.Projection;
            outRaster.NoDataValue = sourceRaster.NoDataValue;

            int rows = sourceRaster.NumRows;
            int cols = sourceRaster.NumColumns;

            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    double sum = 0;
                    int count = 0;

                    // 3x3 窗口遍历
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            double val = sourceRaster.Value[r + i, c + j];
                            if (val != sourceRaster.NoDataValue)
                            {
                                sum += val;
                                count++;
                            }
                        }
                    }

                    if (count > 0)
                        outRaster.Value[r, c] = sum / count;
                    else
                        outRaster.Value[r, c] = outRaster.NoDataValue;
                }
            }
            return outRaster;
        }

        /// <summary>
        /// 2. 实现论文中的差值计算 (Local Difference)
        /// DEM_diff = DEM_orig - DEM_smooth
        /// </summary>
        public IRaster CalculateRasterDifference(IRaster original, IRaster smooth, string outFileName)
        {
            IRaster diffRaster = _fileRepo.CreateResultRaster(outFileName, original.NumColumns, original.NumRows, original.DataType, new string[0]);
            diffRaster.Bounds = original.Bounds.Copy();
            diffRaster.Projection = original.Projection;
            diffRaster.NoDataValue = original.NoDataValue;

            for (int r = 0; r < original.NumRows; r++)
            {
                for (int c = 0; c < original.NumColumns; c++)
                {
                    double valOrig = original.Value[r, c];
                    double valSmooth = smooth.Value[r, c];

                    if (valOrig != original.NoDataValue && valSmooth != smooth.NoDataValue)
                    {
                        diffRaster.Value[r, c] = valOrig - valSmooth;
                    }
                    else
                    {
                        diffRaster.Value[r, c] = diffRaster.NoDataValue;
                    }
                }
            }
            return diffRaster;
        }

        /// <summary>
        /// 3. 计算矢量耐受性测度 (VRM)
        /// 基于 Sappington et al. (2007) 的方法，如论文所述
        /// </summary>
        public IRaster CalculateVRM(IRaster dem, string outFileName)
        {
            int rows = dem.NumRows;
            int cols = dem.NumColumns;
            double cellSize = dem.CellWidth; 

            // 初始化 X, Y, Z 分量数组 
            double[,] rX = new double[rows, cols];
            double[,] rY = new double[rows, cols];
            double[,] rZ = new double[rows, cols];

            // 步骤 A: 计算每个单元格的坡度(Slope)和坡向(Aspect)，并转换为向量
            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    // 使用三阶反距离平方权重的 3x3 窗口计算梯度 (Horn's algorithm)
                    double z1 = dem.Value[r - 1, c - 1]; double z2 = dem.Value[r - 1, c]; double z3 = dem.Value[r - 1, c + 1];
                    double z4 = dem.Value[r, c - 1]; double z6 = dem.Value[r, c + 1];
                    double z7 = dem.Value[r + 1, c - 1]; double z8 = dem.Value[r + 1, c]; double z9 = dem.Value[r + 1, c + 1];

                    if (z1 == dem.NoDataValue || z9 == dem.NoDataValue) continue; // 简单处理边界NoData

                    // 计算 x 和 y 方向的梯度
                    double dz_dx = ((z3 + 2 * z6 + z9) - (z1 + 2 * z4 + z7)) / (8 * cellSize);
                    double dz_dy = ((z7 + 2 * z8 + z9) - (z1 + 2 * z2 + z3)) / (8 * cellSize);

                    // 坡度 (Slope)
                    double slope = Math.Atan(Math.Sqrt(dz_dx * dz_dx + dz_dy * dz_dy));

                    // 坡向 (Aspect)
                    double aspect = 0;
                    if (dz_dx != 0)
                    {
                        aspect = Math.Atan2(dz_dy, -dz_dx);
                        if (aspect < 0) aspect += 2 * Math.PI;
                    }

                    // 将坡度和坡向分解为 3D 向量 (x, y, z)
                    // x = sin(slope) * sin(aspect)
                    // y = sin(slope) * cos(aspect)
                    // z = cos(slope)
                    rX[r, c] = Math.Sin(slope) * Math.Sin(aspect);
                    rY[r, c] = Math.Sin(slope) * Math.Cos(aspect);
                    rZ[r, c] = Math.Cos(slope);
                }
            }

            // 创建结果栅格
            IRaster vrmRaster = _fileRepo.CreateResultRaster(outFileName, cols, rows, typeof(double), new string[0]);
            vrmRaster.Bounds = dem.Bounds.Copy();
            vrmRaster.Projection = dem.Projection;
            vrmRaster.NoDataValue = -1;

            // 步骤 B: 计算 3x3 邻域内的向量结果
            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    double sumX = 0;
                    double sumY = 0;
                    double sumZ = 0;
                    int n = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            sumX += rX[r + i, c + j];
                            sumY += rY[r + i, c + j];
                            sumZ += rZ[r + i, c + j];
                            n++;
                        }
                    }

                    if (n > 0)
                    {
                        // 计算合向量的模 (Magnitude)
                        double magnitude = Math.Sqrt(sumX * sumX + sumY * sumY + sumZ * sumZ);
                        // VRM = 1 - (|r| / n)
                        vrmRaster.Value[r, c] = 1 - (magnitude / n);
                    }
                    else
                    {
                        vrmRaster.Value[r, c] = vrmRaster.NoDataValue;
                    }
                }
            }

            return vrmRaster;
        }

        /// <summary>
        /// 4. 综合方法：计算 VRML (Vector Ruggedness Measure Local) 
        /// </summary>
        public IRaster CalculateVRML(IRaster sourceDem, string savePath)
        {
            // 1. 平滑
            string tempSmoothPath = System.IO.Path.ChangeExtension(savePath, "_smooth.bgd");
            IRaster smoothDem = CalculateMeanFilter(sourceDem, tempSmoothPath);

            // 2. 差值 (Local Difference)
            string tempDiffPath = System.IO.Path.ChangeExtension(savePath, "_diff.bgd");
            IRaster diffDem = CalculateRasterDifference(sourceDem, smoothDem, tempDiffPath);

            // 3. 计算 VRM
            IRaster vrmlResult = CalculateVRM(diffDem, savePath);

            // 清理临时文件（可选，在此处保留以便调试）
            smoothDem.Close();
            diffDem.Close();

            return vrmlResult;
        }
   

        public ColorScheme GetVRMLScheme(IRaster raster)
        {
            ColorScheme scheme = new ColorScheme();
            if (raster.Maximum == 0 && raster.Value[0, 0] == 0)
            {
                raster.GetStatistics();
            }
            double maxVal = raster.Maximum;
            double topBound = Math.Max(maxVal, 0.1);

            // 1. 极平坦区域 (0 - 0.001): 浅蓝色
            ColorCategory cat1 = new ColorCategory(0, 0.001, Color.LightBlue, Color.LightBlue);
            cat1.LegendText = "0 - 0.001 (Very Smooth)";
            scheme.AddCategory(cat1);

            // 2. 微小起伏 (0.001 - 0.005): 绿色
            ColorCategory cat2 = new ColorCategory(0.001, 0.005, Color.LightGreen, Color.LightGreen);
            cat2.LegendText = "0.001 - 0.005 (Smooth)";
            scheme.AddCategory(cat2);

            // 3. 中等崎岖 (0.005 - 0.01): 黄色
            ColorCategory cat3 = new ColorCategory(0.005, 0.01, Color.Yellow, Color.Yellow);
            cat3.LegendText = "0.005 - 0.01 (Moderate)";
            scheme.AddCategory(cat3);

            // 4. 较高崎岖 (0.01 - 0.02): 橙色
            ColorCategory cat4 = new ColorCategory(0.01, 0.02, Color.Orange, Color.Orange);
            cat4.LegendText = "0.01 - 0.02 (Rugged)";
            scheme.AddCategory(cat4);

            // 5. 极度崎岖 (> 0.02): 红色
            ColorCategory cat5 = new ColorCategory(0.02, topBound + 1.0, Color.Red, Color.Red); // 上限加1.0确保覆盖所有高值
            cat5.LegendText = "> 0.02 (Very Rugged)";
            scheme.AddCategory(cat5);

            return scheme;
        }
    }
}