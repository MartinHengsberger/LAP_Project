//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardGame.DAL.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblorder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblorder()
        {
            this.tblcollection = new HashSet<tblcollection>();
        }
    
        public int idorder { get; set; }
        public Nullable<System.DateTime> orderdate { get; set; }
        public Nullable<int> fkperson { get; set; }
        public Nullable<int> fkpack { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblcollection> tblcollection { get; set; }
        public virtual tblpack tblpack { get; set; }
        public virtual tblperson tblperson { get; set; }
    }
}
