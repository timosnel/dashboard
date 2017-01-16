import * as React from "react";

import { BuildOverview } from "./BuildOverview";

interface IAppProps {
}

interface IAppState {
}

export class App extends React.Component<IAppProps, IAppState> {

    constructor(props: IAppProps) {
        super(props);
    }

    public render() {
        return (
            <div className="dashboard-overview">
                <BuildOverview />
            </div>
        );
    }
}