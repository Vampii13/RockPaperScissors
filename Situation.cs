//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RockPaperScissors
{
    using System;
    using System.Collections.Generic;
    
    public partial class Situation
    {
        public int Sieger { get; set; }
        public int Verlierer { get; set; }
        public int Id { get; set; }
    
        public virtual Zeichen Zeichen { get; set; }
        public virtual Zeichen Zeichen1 { get; set; }
    }
}
