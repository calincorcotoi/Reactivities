import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import ActicityFilters from "./ActicityFilters";

function ActivityDashboard() {
  const { activityStore } = useStore();

  const { loadActivities, activityRegistry } = activityStore;

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities();
  }, [activityRegistry.size, loadActivities]);

  if (activityStore.loadingInitial)
    return (
      <LoadingComponent content="Loading activities..."></LoadingComponent>
    );

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width={6}>
        <ActicityFilters />
      </Grid.Column>
    </Grid>
  );
}

export default observer(ActivityDashboard);
