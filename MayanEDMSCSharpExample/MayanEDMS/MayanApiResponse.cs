using System;

namespace MayanEDMSCSharpExample.MayanEDMS
{
    public class MayanApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public int Code { get; set; } = 1;
        public string Exception { get; private set; }
        public T Data { get; set; }

        public MayanApiResponse()
        {

        }

        public MayanApiResponse(T obj)
        {
            this.Data = obj;
        }

        public MayanApiResponse(string exception)
        {
            this.SetException(exception);
        }

        public MayanApiResponse(Exception e)
        {
            Exception = e.Message;
            Success = false;
        }

        public MayanApiResponse(T obj, Exception e)
        {
            Exception = e.Message;
            Success = false;
            this.Data = obj;
        }

        public MayanApiResponse(T obj, string exception)
        {
            this.Data = obj;
            this.SetException(exception);
        }

        public void SetException(Exception e)
        {
            Exception = e.Message;
            Success = false;
        }

        public void SetException(string e)
        {
            Exception = e;
            Success = false;
        }

    }
}
