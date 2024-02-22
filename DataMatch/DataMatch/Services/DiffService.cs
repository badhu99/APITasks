using DataMatch.Enums;
using DataMatch.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DataMatch.Services
{
    public interface IDiffService
    {
        void SetData(int id, DiffDirection direction, string data);
        DataMatchResponse? DataMatch(int id);
        bool ValidateBase64Encoded (string data);
    }
    public class DiffService : IDiffService
    {
        private readonly IMemoryCache _memoryCache;

        public DiffService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DataMatchResponse? DataMatch(int id)
        {
            DataMatchResponse response = new();

            if (_memoryCache.TryGetValue(id, out DiffModel model) == false || model == null)
                return null;

            if (string.IsNullOrWhiteSpace(model.Right) || string.IsNullOrWhiteSpace(model.Left))
                return null;

            if (model.Left.Length != model.Right.Length)
            {
                response.DiffResultType = "SizeDoNotMatch";
                return response;
            }

            if (string.Equals(model.Left, model.Right, StringComparison.Ordinal))
            {
                response.DiffResultType = "Equals";
                return response;
            }

            List<Diff> diffs = new();
            for (int i = 0; i < model.Left.Length; i++)
            {
                if (model.Left[i] != model.Right[i])
                {
                    int length = 1;
                    while (model.Left[i + length] != model.Right[i + length] && i + length < model.Left.Length)
                        length++;

                    diffs.Add(new Diff { Offset = i, Length = length });
                    i += length - 1;
                }
            }

            response.DiffResultType = "ContentDoNotMatch";
            response.Diffs = diffs;
            return response;
        }

        public void SetData(int id, DiffDirection direction, string data)
        {
            if(_memoryCache.TryGetValue(id, out DiffModel model) == false || model == null)
                model = new DiffModel();

            model.Left = direction == DiffDirection.Left ? data : model.Left;
            model.Right = direction == DiffDirection.Right ? data : model.Right;

            _memoryCache.Set(id, model);
        }

        public bool ValidateBase64Encoded(string data)
        {
            try
            {
                byte[] result = Convert.FromBase64String(data);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
