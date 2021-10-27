using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC2.BO;
using MyLibs.Serialisation;

namespace MC.DAL
{
    public class BaseRepository<T> where T: User
    {
        protected List<T> datas;
        private readonly string PATH = $"Data/{typeof(T).Name}.json";
        private Serializer<List<T>> serializer;
        public BaseRepository()
        {
            datas = new List<T>();
            FileInfo fi = new FileInfo(PATH);

            if (!fi.Directory.Exists)
                fi.Directory.Create();

            serializer = new Serializer<List<T>>(Mode.JSON, PATH);
            Restore();
        }

        private int Check(T Obj)
        {
            var index = -1;
            for (int i = 0; i < datas.Count; i++)
                if (Obj.Equals(datas[i]))
                    index = i;
            return index;
        }

        public void Add(T obj)
        {
            int index = Check(obj);
            if (index != -1)
                throw new DuplicateWaitObjectException($"{obj.Email} already exists !");

            datas.Add(obj);
            Save();
        }
        public void Set(T oldObj, T newObj)
        {
            int oldIndex = Check(oldObj);
            if (oldIndex < 0)
                throw new KeyNotFoundException($"{oldObj.Email} not found !");

            var newIndex = Check(newObj);

            if (newIndex >= 0 && newIndex != oldIndex)
                throw new KeyNotFoundException($"{oldObj.Email} already exists !");

            datas[oldIndex] = newObj;
        }
        public void Delete(T obj)
        {
            var index = Check(obj);

            if (index >= 0)
                datas.RemoveAt(index);
            Save();
        }

        public void Save()
        {
            serializer.Serialize(datas);
        }
        public void Restore()
        {
            FileInfo fi = new FileInfo(PATH);
            if (fi.Exists && fi.Length > 0)
                datas = serializer.Deserialize();
        }

        public List<T> FindByName(string value)
        {
            List<T> list = new List<T>();
            foreach (var data in datas)
                if (data.Fullname.ToLower().Contains(value.ToLower()))
                    list.Add(data);
            return new List<T>(list);
        }
        public List<T> GetAll()
        {
            Restore();
            T[] items = new T[datas.Count];
            datas.CopyTo(items);
            return items.ToList<T>();
        }

    }
}
