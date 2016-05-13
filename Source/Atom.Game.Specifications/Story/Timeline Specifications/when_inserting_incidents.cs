    
namespace Atom.Story.Specifications.TimelineSpecifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Collections;
    using Machine.Specifications;

    [Subject( typeof( Timeline ) )]
    public class when_inserting_incidents
    {
        static Timeline timeline;
        static IList<Incident> incidents;

        Establish context = () => {
            timeline = new Timeline();
            incidents = new Incident[] { 
                new Incident() { RelativeTick = new TimeTick( 0 ) },
                new Incident() { RelativeTick = new TimeTick( 20 ) },
                new Incident() { RelativeTick = new TimeTick( 0 ) },
                new Incident() { RelativeTick = new TimeTick( 10 ) }
            };
        };

        Because of = () => {
            foreach( var incident in incidents )
            {
                timeline.Insert( incident );
            }
        };

        It contains_all_of_the_inserted_incidents = () =>
            timeline.Incidents.ShouldContainOnly( incidents );

        It should_have_created_frames_in_the_correct_order = () => {
            timeline.Frames.Select( frame => frame.RelativeTick.Value )
                .ElementsEqual( new uint[] { 0, 10, 20 } )
                .ShouldBeTrue();
        };

        It contains_the_incidents_sorted_per_frame = () => {
            timeline.GetIncident( 0, 0 ).ShouldEqual( incidents[0] );
            timeline.GetIncident( 2, 0 ).ShouldEqual( incidents[1] );
            timeline.GetIncident( 0, 1 ).ShouldEqual( incidents[2] );
            timeline.GetIncident( 1, 0 ).ShouldEqual( incidents[3] );
        };
    }
}
