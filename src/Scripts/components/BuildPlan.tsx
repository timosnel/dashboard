import * as React from "react";

const enum BuildStatus {
    Successful,
    Failed,
    Other
}

interface IBuildPlanProps {
    planKey: string;
    name: string; 
}

interface IBuildPlanState {
    status: BuildStatus;
}

export class BuildPlan extends React.Component<IBuildPlanProps, IBuildPlanState> {

    // Holds the interval used to fetch build status.
    private interval: number;

    constructor(props: IBuildPlanProps) {
        super(props);
        this.state = { status: BuildStatus.Other };
    }

    private getStatus(): void {
        fetch("/api/bamboo/buildplan/" + this.props.planKey + "/status")
        .then(rawResponse => {
            rawResponse.json().then((status: string) => {
                
                let buildStatus: BuildStatus;
                
                switch(status) {
                    case "Successful":
                        buildStatus = BuildStatus.Successful;
                        break;
                    case "Failed":
                        buildStatus = BuildStatus.Failed;
                        break;
                    default:
                        buildStatus = BuildStatus.Other;
                        break;
                }

                this.setState({ status: buildStatus });
            });
        })
        .catch(error => {
            console.log("Error occurred while retrieving build plans: " + error);
        });
    }

    public shouldComponentUpdate(nextProps: IBuildPlanProps, nextState: IBuildPlanState) {
        return (
            nextProps.planKey !== this.props.planKey ||
            nextProps.name !== this.props.name ||
            nextState.status !== this.state.status
        );
    }

    public componentDidMount() {
        this.interval = setInterval(() => this.getStatus(), 30000);  
    }

    public componentWillUnmount() {
        clearInterval(this.interval);
    }

    public render() {
        var statusClass: string;

        switch (this.state.status) {
            case BuildStatus.Successful:
                statusClass = "successful";
                break;
            case BuildStatus.Failed:
                statusClass = "failed";
                break;
            default:
                statusClass = "other";
                break;
        }

        return (
            <div className={"build " + statusClass}>
                <h1>{this.props.name}</h1>
            </div>
        );
    }
}