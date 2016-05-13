
namespace Atom.Story.Specifications.TimelineSpecifications
{
    using Machine.Specifications;

    [Subject( typeof( Timeline ) )]
    public class when_newly_created
    {
        static Timeline timeline;

        Because of = () =>
            timeline = new Timeline();

        It should_be_active = () =>
            timeline.IsActive.ShouldBeTrue();

        It should_have_no_name = () =>
            timeline.Name.ShouldBeNull();

        It should_have_no_frames = () =>
            timeline.FrameCount.ShouldEqual( 0 );

        It should_have_no_incidents = () =>
            timeline.Incidents.ShouldBeEmpty();

        It should_have_a_start_offset_of_zero_ticks = () =>
            timeline.StartOffset.ShouldEqual(TimeTick.Zero);
    }
}
