//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kurssitietokanta.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vastuuopettajat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vastuuopettajat()
        {
            this.Tutkinnonosat = new HashSet<Tutkinnonosat>();
        }
    
        public int Vastuuopettajan_Id { get; set; }
        public string Vastuuopettajanimi { get; set; }
        public Nullable<int> Opettajan_Id { get; set; }
    
        public virtual Opettajat Opettajat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tutkinnonosat> Tutkinnonosat { get; set; }
    }
}
