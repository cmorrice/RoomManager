using Intel.Unite.Common.Command.Serialize;
using UnitePlugin.Constants;

namespace UnitePlugin.Model.Command
{
    public class CommandWraper<T> : BaseCommand<T>
    {
        public CommandWraper(T t) : base( (ICommandSerializer)new JsonCommandSerializer(), t, ModuleConstants.ModuleInfo.Id )
        { }
    }
}
