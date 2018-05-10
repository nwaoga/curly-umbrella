namespace FootieWorld.data.ef.UnitOfWork
{
    public interface IUnitOfWork
    {
        FootieDbEntities Context { get; set; }
        void SaveChanges();
        void Dispose();
    }
}