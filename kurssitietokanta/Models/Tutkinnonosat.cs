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
    
    public partial class Tutkinnonosat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tutkinnonosat()
        {
            this.Lukukaudet = new HashSet<Lukukaudet>();
            this.Opintojaksot = new HashSet<Opintojaksot>();
        }
    
        public int Tutkintoosa_Id { get; set; }
        public string Tutkintoosanimi { get; set; }
        public int Tutkinto_Id { get; set; }
        public int Vastuuopettajan_Id { get; set; }
        public string LaajuusOSP { get; set; }
        public string Esitietovaatimus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lukukaudet> Lukukaudet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opintojaksot> Opintojaksot { get; set; }
        public virtual Tutkinnot Tutkinnot { get; set; }
        public virtual Vastuuopettajat Vastuuopettajat { get; set; }
    }
}