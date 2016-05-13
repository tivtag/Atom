
namespace Atom.Story.Specifications.StoryboardSpecifications
{
    using Machine.Specifications;
    using Atom.Moles;

    [Subject( typeof( Storyboard ) )]
    public class when_updated
    {
        //       +---*->         timeline B
        //       |
        //   +-*-|-----*->       timeline A
        // __|___|_____________
        // 0 1 2 3 4 5 6 7 8 9   seconds

        // move from 0 to 5 seconds

        // *        is an incident / event
        // +------> is a timeline
        // ______   is a storyboard
        
        static Storyboard storyboard;
        static IUpdateContext updateContext;
        static Timeline timelineA;
        static Timeline timelineB;
        static bool incidentAOneHandled;
        static bool incidentATwoHandled;
        static bool incidentBOneHandled;

        Establish context = () => {
            updateContext = new SIUpdateContext() { 
                 FrameTimeGet = () => 5.5f
            };

            timelineA = new Timeline() { 
                Name = "A",
                StartOffset = TimeTick.FromSeconds(1)
            };

            timelineA.Insert(
                new Incident() {
                    RelativeTick = TimeTick.FromSeconds(1),
                    Action = new SIAction() {
                         Execute = () => incidentAOneHandled = true
                    }
                }
            );

            timelineA.Insert(
                new Incident() {
                    RelativeTick = TimeTick.FromSeconds( 5 ),
                    Action = new SIAction() {
                        Execute = () => incidentATwoHandled = true
                    }
                }
            );

            timelineB = new Timeline() {
                Name = "B",
                StartOffset = TimeTick.FromSeconds( 3 )
            };

            timelineB.Insert(
                new Incident() {
                    RelativeTick = TimeTick.FromSeconds( 2 ),
                    Action = new SIAction() {
                        Execute = () => incidentBOneHandled = true
                    }
                }
            );

            storyboard = new Storyboard();
            storyboard.AddTimeline( timelineA );
            storyboard.AddTimeline( timelineB );
        };

        Because of = () =>
            storyboard.Update( updateContext );

        It should_handled_the_relevant_incidents = () => {
            incidentAOneHandled.ShouldBeTrue();
            incidentATwoHandled.ShouldBeFalse();
            incidentBOneHandled.ShouldBeTrue();
        };
    }
}
