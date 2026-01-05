// DAL/FileRepository.cs
using System;
using System.Windows.Forms;
using DotSpatial.Data;

namespace 集成.DAL
{
    public class FileRepository
    {
        // 保存矢量数据 (FeatureSet)
        public void SaveShapefile(IFeatureSet fs, string defaultName)
        {
            if (fs == null || fs.Features.Count == 0)
                throw new Exception("没有可保存的要素。");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = defaultName;
            sfd.Filter = "Shapefiles (*.shp)|*.shp";

            // 注意：DAL 层通常不应该弹出 UI (MessageBox/Dialog)，
            // 但为了适应你当前的 WinForms 简单迁移，我们可以先保留，
            // 更标准的做法是 UI 层传路径进来，DAL 只管保存。
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fs.SaveAs(sfd.FileName, true);
            }
        }

        // 保存/创建栅格文件
        public void SaveRaster(IRaster raster)
        {
            raster.Save();
        }

        // 创建新的空白栅格文件（用于栅格计算结果）
        public IRaster CreateResultRaster(string fileName, int cols, int rows, Type dataType, string[] options)
        {
            return Raster.CreateRaster(fileName, null, cols, rows, 1, dataType, options);
        }
    }
}