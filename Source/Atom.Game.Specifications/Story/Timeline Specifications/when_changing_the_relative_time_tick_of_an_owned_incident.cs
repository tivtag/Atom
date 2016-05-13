
namespace Atom.Story.Specifications.TimelineSpecifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Collections;
    using Machine.Specifications;

    [Subject( typeof( Timeline ) )]
    public class when_changing_the_relative_time_tick_of_an_owned_incident
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

        Because of = () =>
            incidentB.RelativeTick = 0;

        It should_have_kept_all_frames = () =>
            timeline.Frames.Count().ShouldEqual( 2 );

        It should_not_have_the_incidents_resorted = () =>
            timeline.Frames.SelectMany( f => f.Incidents )
                .Select( i => i.RelativeTick.Value )
                .ElementsEqual( new uint[] { 0, 10 } );
    }
}
