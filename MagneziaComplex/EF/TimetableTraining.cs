//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MagneziaComplex.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimetableTraining
    {
        public int idTimetableTraining { get; set; }
        public int idEmployee { get; set; }
        public int idTraining { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public int idClub { get; set; }
    
        public virtual Club Club { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Training Training { get; set; }
    }
}