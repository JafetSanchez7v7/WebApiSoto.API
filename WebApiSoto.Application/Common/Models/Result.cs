using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class Result<T>  
    {
        
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? Message { get; set; }
        
        public static Result<T> Success(T value , int statusCode) => new () { StatusCode = statusCode, Value = statusCode == 204?  default : value, IsSuccess= true, Message = "Exito" };
        public static Result<T> Failure(bool isSucces, string Message, int statusCode) => new () {  StatusCode = statusCode ,IsSuccess = isSucces, Message = Message };

    }
}
