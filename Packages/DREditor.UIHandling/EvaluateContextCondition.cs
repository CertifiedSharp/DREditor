//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EvaluateContextCondition<T> : EvaluateCondition
    {
        /// <summary>
        ///
        /// </summary>
        protected T context;


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <remarks></remarks>
        public void InvokeEvaluate(T context) => Evaluate(context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Evaluate(T context)
        {
            this.context = context;

            bool result = Evaluate();

            ClearContext();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected void ClearContext() => context = default;
    }
}