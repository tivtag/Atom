
namespace Atom.Story.Specifications.IncidentSpecifications
{
    using Machine.Specifications;

    [Subject( typeof( Incident ) )]
    public class when_newly_created
    {
        static Incident incident;

        Because of = () =>
            incident = new Incident();

        It should_have_no_action = () =>
            incident.Action.ShouldBeNull();

        It should_be_positioned_at_the_start_of_tick_time = () =>
            incident.RelativeTick.ShouldEqual( TimeTick.Zero );

        It should_be_doable_without_error = () =>
            incident.Do();

        It should_be_undoable_without_error = () =>
            incident.Undo();

        It should_lie_before_ticktime_one = () =>
            incident.IsBefore( new TimeTick( 1 ) ).ShouldBeTrue();

        It should_not_lie_after_ticktime_zero = () =>
            incident.IsAfter( TimeTick.Zero ).ShouldBeFalse();

        It should_exactly_lie_within_ticktime_zero = () =>
            incident.IsWithin( TimeTick.Zero, tolerance: TimeTick.Zero ).ShouldBeTrue();
    }
}
