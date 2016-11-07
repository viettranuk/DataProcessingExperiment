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
    
    public partial class UploadedFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UploadedFile()
        {
            this.TaxDetails = new HashSet<TaxDetail>();
            this.UnprocessedDetails = new HashSet<UnprocessedDetail>();
        }
    
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public System.DateTime ImportedOn { get; set; }
        public long ProcessedLineCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxDetail> TaxDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnprocessedDetail> UnprocessedDetails { get; set; }
        public virtual UploadedFilesContent UploadedFilesContent { get; set; }
    }
}