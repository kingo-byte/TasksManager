using DAL.DapperAccess;

namespace BAL
{
    public partial class BAL
    {
        private readonly DapperAccess _dapperAccess;
        public BAL(DapperAccess dapperAccess)
        {
            _dapperAccess = dapperAccess;
            InitializeEvents();
        }
    }
}
