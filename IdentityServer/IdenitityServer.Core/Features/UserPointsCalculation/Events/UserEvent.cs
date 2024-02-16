using System;

namespace IdenitityServer.Core.Features.UserPointsCalculation.Events
{
    public interface IUserPointsEvent
    {
    }

    public record UserPointsAdded(string userId,double points,DateTime createdDate) : IUserPointsEvent;

    public record UserPointsSpent(string userId, double points, DateTime spentDate) : IUserPointsEvent;

    public record UserPointsAdjusted(string userId, double points, DateTime adjustedDate,string adminId) : IUserPointsEvent;
}
