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
    
    public partial class vCollectionCards
    {
        public int idcard { get; set; }
        public int fkperson { get; set; }
        public string cardname { get; set; }
        public byte mana { get; set; }
        public short attack { get; set; }
        public short life { get; set; }
        public int fkorder { get; set; }
        public int idcollectioncard { get; set; }
        public byte[] pic { get; set; }
    }
}
