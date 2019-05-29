using Never;
using Never.Aop;
using Never.Commands;
using Never.DataAnnotations;
using Never.Exceptions;
using B2C.Message.Contract.Commands;
using B2C.Message.Core.Domains;
using B2C.Message.Core.Repository;
using System;

namespace B2C.Message.Core.Handlers
{
    [Logger]
    public class EmailCodeCommandHandler : ICommandHandler<CreateEmailCodeCommand>, ICommandHandler<DestroyEmailCodeCommand>
    {
        #region field

        private readonly IEmailCodeRepository repository = null;

        #endregion field

        #region ctor

        public EmailCodeCommandHandler(IEmailCodeRepository repository)
        {
            this.repository = repository;
        }

        #endregion ctor

        public ICommandHandlerResult Execute(ICommandContext context, DestroyEmailCodeCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => { return this.repository.Rebuild(command.Email, command.UsageType); });
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists, "验证失败");

            var validation = root.Destroy(context, command);
            if (!root.CanCommit())
                return context.CreateResult(validation.HasValue && validation.Value.Option == ValidationOption.Continue ? CommandHandlerStatus.NothingChanged : CommandHandlerStatus.Fail, validation.HasValue ? validation.Value.ErrorMessage : "");

            if (this.repository.Destroy(root) <= 0)
                throw new RepositoryExcutingException("验证失败,请稍后再尝试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, CreateEmailCodeCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => EmailCodeAggregateRoot.Register(context, command));
            if (!root.CanCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (this.repository.Save(root) <= 0)
                throw new RepositoryExcutingException("获取验证码失败，请稍后再尝试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }
    }
}