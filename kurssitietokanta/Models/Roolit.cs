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
    
    public partial class Roolit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roolit()
        {
            this.Käyttöoikeudet = new HashSet<Käyttöoikeudet>();
            this.Login = new HashSet<Login>();
        }
    
        public int Rooli_Id { get; set; }
        public string Roolinimi { get; set; }
        public string Roolityyppi { get; set; }
        public string Kuvaus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Käyttöoikeudet> Käyttöoikeudet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Login> Login { get; set; }
    }
}