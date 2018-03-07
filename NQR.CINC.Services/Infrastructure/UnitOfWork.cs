using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.PatientService;
using NQR.CINC.Services.UserService;
using System;
using System.ComponentModel.Composition;

namespace NQR.CINC.Services.Infrastructure
{
    [Export]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class UnitOfWork : IDisposable
    {

        private CINCEntities _context;

        const string _dbContextKey = "_dbcontext";

        public UnitOfWork()
        {
            _context = CINCEntities.Current;
        }
        public CINCEntities Context
        {
            get { return _context; }
        }


        //private IChunkRepository _chunkRepositoryInstance;

        //[Import(typeof(IChunkRepository))]
        //public IChunkRepository ChunkRepositoryInstance
        //{
        //    get
        //    {
        //        //if (this._chunkRepositoryInstance == null)
        //        //{
        //        //    this._chunkRepositoryInstance = new ChunkRepository();
        //        //    this._chunkRepositoryInstance.HostUrl = HostUrl;
        //        //}
        //        return _chunkRepositoryInstance;
        //    }
        //    set
        //    {
        //        _chunkRepositoryInstance = value;
        //    }

        private IUserRepository _userRepositoryInstance;
        [Import(typeof(IUserRepository))]
        public IUserRepository UserRepositoryInstance
        {
            get
            {
                if (this._userRepositoryInstance == null)
                {
                    this._userRepositoryInstance = new UserRepository();

                }
                return _userRepositoryInstance;
            }
            set
            {
                _userRepositoryInstance = value;
            }
        }

        private IRepository<User> _UserEntity;

        public IRepository<User> UserEntity
        {
            get
            {
                if (this._UserEntity == null)
                {
                    this._UserEntity = new GenericDBRepository<User>(_context);
                }
                return _UserEntity;
            }
        }

        private IPatientRepository _patientRepositoryInstance;
        [Import(typeof(IPatientRepository))]
        public IPatientRepository PatientRepositoryInstance
        {
            get
            {
                if (this._patientRepositoryInstance == null)
                {
                    this._patientRepositoryInstance = new PatientRepository();

                }
                return _patientRepositoryInstance;
            }
            set
            {
                _patientRepositoryInstance = value;
            }
        }

        private IRepository<Patient> _PatientEntity;

        public IRepository<Patient> PatientEntity
        {
            get
            {
                if (this._PatientEntity == null)
                {
                    this._PatientEntity = new GenericDBRepository<Patient>(_context);
                }
                return _PatientEntity;
            }
        }

        public void Save()
        {
            //Todo : Uncomment the following line once the code is integrated into the project
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    CINCEntities.DisposeCurrent();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
