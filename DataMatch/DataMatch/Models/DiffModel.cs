namespace DataMatch.Models
{
    public class DiffModel
    {
        private byte[] _rightDecoded = Array.Empty<byte>();
        private byte[] _leftDecoded = Array.Empty<byte>();

        public string Left
        {
            get
            {
                return Convert.ToBase64String(_leftDecoded) ?? "";
            }
            set { _leftDecoded = Convert.FromBase64String(value); }
        }
        public string Right
        {
            get
            {
                return Convert.ToBase64String(_rightDecoded) ?? "";
            }
            set {
                _rightDecoded = Convert.FromBase64String(value);
            }
        }

        public DataMatchResponse Compare()
        {
            DataMatchResponse response = new();
            if (_rightDecoded.Length != _leftDecoded.Length)
            {
                response.DiffResultType = "SizeDoNotMatch";
                return response;
            }

            bool boolLeftRight = true;
            List<Diff> diffs = new();
            for (int i = 0; i < _rightDecoded.Length; i++)
            {
                if (_leftDecoded[i] != _rightDecoded[i])
                {
                    int length = 1;
                    while (i + length < _leftDecoded.Length && _leftDecoded[i + length] != _rightDecoded[i + length])
                        length++;
                    diffs.Add(new Diff { Offset = i, Length = length });
                    i += length - 1;
                    boolLeftRight = false;
                }
            }

            response.DiffResultType = boolLeftRight ? "Equals" : "ContentDoNotMatch";
            response.Diffs = boolLeftRight ? null : diffs;
            return response;
        }
    }
}
