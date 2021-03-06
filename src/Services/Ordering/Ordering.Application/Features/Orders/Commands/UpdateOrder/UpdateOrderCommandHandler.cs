using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var ordertoUpdate = await orderRepository.GetByIdAsync(request.Id);
            if (ordertoUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            mapper.Map(request, ordertoUpdate, typeof(UpdateOrderCommand), typeof(Order));

            await orderRepository.UpdateAsync(ordertoUpdate);

            logger.LogInformation("Order {orderId} is successfully updated.", ordertoUpdate.Id);

            return Unit.Value;
        }
    }
}