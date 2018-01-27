namespace TIPS.Entities.TransportEntities
{
    public class VehicleFuelQuantityChart_vw
    {
        public virtual long Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string VehicleId { get; set; }
        public virtual int Year { get; set; }
        public virtual decimal Jan { get; set; }
        public virtual decimal Feb { get; set; }
        public virtual decimal Mar { get; set; }
        public virtual decimal Apr { get; set; }
        public virtual decimal May { get; set; }
        public virtual decimal Jun { get; set; }
        public virtual decimal Jul { get; set; }
        public virtual decimal Aug { get; set; }
        public virtual decimal Sep { get; set; }
        public virtual decimal Oct { get; set; }
        public virtual decimal Nov { get; set; }
        public virtual decimal Dec { get; set; }
        public virtual decimal TotalFuelQuantity { get; set; }
    }
}
