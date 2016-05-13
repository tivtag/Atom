
namespace Atom.Story.Specifications.TimelineSpecifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Collections;
    using Machine.Specifications;

    [Subject( typeof( Timeline ) )]
    public class when_changing_the_relative_time_tick_of_an_owned_incident_and_then_rebuilding_it
    {
        static Timeline timeline;
        static Incident incidentA, incidentB;

        Establish context = () => {
            timeline = new Timeline();
            incidentA = new Incident() { RelativeTick = 0 };
            incidentB = new Incident() { RelativeTick = 10 };

            timeline.Insert( incidentA );
            timeline.Insert( incidentB );
        };

        Because of = () => {
            incidentB.RelativeTick = 0;
            timeline.Rebuild();
        };

        It should_have_removed_empty_frames = () =>
            timeline.Frames.Count().ShouldEqual( 1 );

        It should_have_the_incidents_sorted = () =>
            timeline.Frames.ElementAt( 0 ).Incidents.ShouldContainOnly( incidentA, incidentB );      

    }
}
