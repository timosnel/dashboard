import * as React from "react";
import "whatwg-fetch";

import { BuildPlan } from "./BuildPlan";

interface IBuildPlanModel {
    key: string;
    name: string;
}

interface IBuildOverviewState {
    buildPlans: IBuildPlanModel[];
}

export class BuildOverview extends React.Component<undefined, IBuildOverviewState> {

    constructor(props: undefined) {
        super(props);
        this.state = { buildPlans: [] };
    }

    private getBuildPlans(): void {
        fetch("/api/bamboo/buildplan")
        .then(rawResponse => {
            rawResponse.json().then((buildPlans: IBuildPlanModel[]) => {
                this.setState({ buildPlans: buildPlans });
            });
        })
        .catch(error => {
            console.log("Error occurred while retrieving build plans: " + error);
        });
    }

    public componentDidMount() {
        this.getBuildPlans();
    }

    public render() {
        let buildPlans: JSX.Element[] = this.state.buildPlans.map(buildPlan => {
            return <BuildPlan key={buildPlan.key} planKey={buildPlan.key} name={buildPlan.name} />;
        });

        return (
            <div>
                {buildPlans}
            </div>
        );
    }
}