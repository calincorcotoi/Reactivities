import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";

export default observer(function ProfilePage() {
  const { username } = useParams();
  const { profileStore } = useStore();
  const { profile, loadProfile, loadingProfile } = profileStore;

  useEffect(() => {
    if (username)
      loadProfile(username).then(() => {
        console.log(profile);
      });
  }, [loadProfile, username]);

  if (loadingProfile || profile == null) return <LoadingComponent content="Loading profile..." />;

  return (
    <Grid>
      <Grid.Column width={16}>
        <ProfileHeader profile={profile} />
        <ProfileContent profile={profile} />
      </Grid.Column>
    </Grid>
  );
});
