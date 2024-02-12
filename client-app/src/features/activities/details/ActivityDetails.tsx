import { Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import ActivityDetaledHeader from "./ActivityDetaledHeader";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedSidebar from "./ActivityDetailedSidebar";

export default observer(function ActivityDetails() {
  const { activityStore } = useStore();
  const { selectedActivity: activity, loadActivity, loadingInitial, clearSelectedActivity } = activityStore;

  const { id } = useParams();

  useEffect(() => {
    if (id) loadActivity(id);
    return () => {
      clearSelectedActivity();
    };
  }, [id, loadActivity]);

  if (loadingInitial || !activity) return <LoadingComponent></LoadingComponent>;

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetaledHeader activity={activity} />
        <ActivityDetailedInfo activity={activity} />
        <ActivityDetailedChat activityId={activity.id} />
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailedSidebar acticity={activity} />
      </Grid.Column>
    </Grid>
  );
});
