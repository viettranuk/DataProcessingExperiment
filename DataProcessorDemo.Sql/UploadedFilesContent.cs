//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataProcessorDemo.Sql
{
    using System;
    using System.Collections.Generic;
    
    public partial class UploadedFilesContent
    {
        public int FileId { get; set; }
        public byte[] Data { get; set; }
    
        public virtual UploadedFile UploadedFile { get; set; }
    }
}
