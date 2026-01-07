using System;
using DotSpatial.Data;

namespace 集成.DAL
{
    public class FileRepository
    {
        public void SaveShapefile(IFeatureSet fs, string filePath)
        {
            if (fs == null || fs.Features.Count == 0)
                throw new Exception("没有可保存的要素。");
            fs.SaveAs(filePath, true);
        }

        public void SaveRaster(IRaster raster)
        {
            if (raster == null) throw new Exception("栅格数据为空。");
            raster.Save();
        }

        public IRaster CreateResultRaster(string fileName, int cols, int rows, Type dataType, string[] options)
        {
            return Raster.CreateRaster(fileName, null, cols, rows, 1, dataType, options);
        }
    }
}