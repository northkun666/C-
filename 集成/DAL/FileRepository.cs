// 文件位置: 集成/DAL/FileRepository.cs
using System;
using DotSpatial.Data;

namespace 集成.DAL
{
    public class FileRepository
    {
        // 1. 保存矢量数据 (纯粹的保存逻辑，不含弹窗)
        public void SaveShapefile(IFeatureSet fs, string filePath)
        {
            if (fs == null || fs.Features.Count == 0)
                throw new Exception("没有可保存的要素。");

            // 直接保存到传入的路径
            fs.SaveAs(filePath, true);
        }

        // 2. 保存栅格数据
        public void SaveRaster(IRaster raster)
        {
            if (raster == null) throw new Exception("栅格数据为空。");
            raster.Save();
        }

        // 3. 创建新的空白栅格文件
        public IRaster CreateResultRaster(string fileName, int cols, int rows, Type dataType, string[] options)
        {
            return Raster.CreateRaster(fileName, null, cols, rows, 1, dataType, options);
        }
    }
}