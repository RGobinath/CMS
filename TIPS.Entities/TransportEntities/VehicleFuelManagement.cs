using System;
using System.Collections.Generic;
using System.Text;
using TIPS.Entities;
using System.Runtime.Serialization;


namespace TIPS.Entities.TransportEntities
{
    public class VehicleFuelManagement
    {

        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string RefNo{get;set;}
        [DataMember]
        public virtual int RefId
        {
            get;
            set;
        }
        [DataMember]
        public virtual string Campus
        {
            get;
            set;
        }

        [DataMember]
        public virtual string VehicleType
        {
            get;
            set;
        }

        [DataMember]
        public virtual string Type
        {
            get;
            set;
        }
        [DataMember]
        public virtual int VehicleId { get; set; }
        [DataMember]
        public virtual string VehicleNo
        {
            get;
            set;
        }

        [DataMember]
        public virtual decimal? FuelQuantity
        {
            get;
            set;
        }

        [DataMember]
        public virtual string FuelType
        {
            get;
            set;
        }

        [DataMember]
        public virtual string FilledBy
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime? FilledDate
        {
            get;
            set;
        }

        [DataMember]
        public virtual string BunkName
        {
            get;
            set;
        }

        [DataMember]
        public virtual string BunkContactNo
        {
            get;
            set;
        }

        [DataMember]
        public virtual string CreatedBy
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime? CreatedDate
        {
            get;
            set;
        }

        [DataMember]
        public virtual string ModifiedBy{get;set;}

        [DataMember]
        public virtual DateTime? ModifiedDate
        {
            get;
            set;
        }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        public virtual string FuelFillType { get; set; }
        public virtual decimal LastMilometerReading { get; set; }
        public virtual decimal CurrentMilometerReading { get; set; }
        public virtual decimal Mileage { get; set; }
        public virtual decimal LitrePrice { get; set; }
        public virtual decimal TotalPrice { get; set; }
        //[DataMember]
        //public virtual decimal LastMiloMeterReading1 { get; set; }
        //[DataMember]
        //public virtual decimal CurrentMiloMeterReading1 { get; set; }
        [DataMember]
        public virtual decimal Distance { get; set; }
        [DataMember]
        public virtual decimal KMResetValue { get; set; }
        [DataMember]
        public virtual bool IsKMReseted { get; set; }
    }
}
