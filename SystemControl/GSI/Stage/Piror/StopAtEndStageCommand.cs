using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Stage.Piror
{
    public class StopAtEndStageCommand : StageCommand
    {
        public StopAtEndStageCommand(string cmnd, Action<string> doOnPresonse)
            : base(cmnd, 0, doOnPresonse)
        {
            this.StopOnResponse = (rsp) =>
            {
                return rsp.ToLower().Trim() == "end";
            };
        }
    }
}
