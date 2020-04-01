using HeroApp.App.Common.Exceptions;
using HeroApp.Infra;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroApp.App.Common
{
    public class DbTransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IHeroContext heroContext;

        public DbTransactionBehaviour(
            ILogger<TRequest> logger,
            IHeroContext heroContext)
        {
            _timer = new Stopwatch();

            _logger = logger;
            this.heroContext = heroContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            TResponse response = default;
            DbRollBackException rollBackException = null;
            try
            {
                await heroContext.BeginTransactionAsync();
                response = await next();
                await heroContext.CommitTransactionAsync();


            }
            catch (Exception ex)
            {
                heroContext.RollbackTransaction();
                rollBackException = new DbRollBackException(ex);
            }
            finally
            {
                _timer.Stop();
                if (!(rollBackException is null))
                {
                    throw rollBackException;
                }

            }




            return response;
        }
    }
}
