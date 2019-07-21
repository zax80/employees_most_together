using System;

namespace AssignmentTeamLongestPeriod
{
    public class DocumentRecord
    {
        public int EmpID { get; private set; }
        public int ProjectID { get; private set; }
        public DateTime DateFrom { get; private set; }
        public DateTime DateTo { get; private set; }

        public DocumentRecord(int empID, int projectID, DateTime start, DateTime end)
        {
            this.EmpID = empID;
            this.ProjectID = projectID;
            this.DateFrom = start;
            this.DateTo = end;
        }
    }
}