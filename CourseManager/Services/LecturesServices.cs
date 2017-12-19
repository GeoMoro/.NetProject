using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Business;

namespace Services
{
    public class LecturesServices
    {
        public List<string> GetFiles(Guid id)
        {
            var fileList = new List<string>();
            var currentLecture = Get
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Lectures\\" + GetLectureById(id).Title;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var files in Directory.GetFiles(path))
            {
                fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }
    }
}
