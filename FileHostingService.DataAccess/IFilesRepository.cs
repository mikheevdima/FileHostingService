using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess
{
    public interface IFilesRepository
    {
        File Add(File file);
        void Delete(Guid id);
        void UpdateContent(Guid id, byte[] content);
        File GetInfo(Guid id);
        IEnumerable<File> GetUserFiles(Guid id);
        byte[] GetContent(Guid id);
        void GiveAccessToFile(Guid userid, Guid fileid);
        void DeleteAccessToFile(Guid userid, Guid fileid);
        IEnumerable<File> GetAccesibleFiles(Guid userid);
    }
}