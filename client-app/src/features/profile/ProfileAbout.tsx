import { Button, Grid, GridColumn, GridRow, Header, Icon, Tab } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { useState } from "react";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Form, Formik } from "formik";
import MyTextArea from "../../app/common/form/MyTextArea";
import * as Yup from "yup";
import { observer } from "mobx-react-lite";

export default observer(function ProfileAbout() {
  const {
    profileStore: { isCurrentUser, profile, updateProfile },
  } = useStore();

  const [editModeActive, setEditModeActive] = useState(false);

  return (
    <Tab.Pane>
      <Grid>
        <GridRow>
          <GridColumn width={10}>
            <Header>
              <Icon name="user circle" /> About user {profile?.displayName}
            </Header>
          </GridColumn>
          <GridColumn width={6}>
            {isCurrentUser && (
              <Button
                floated="right"
                content={editModeActive ? "Cancel" : "Edit Profile"}
                onClick={() => setEditModeActive(!editModeActive)}
              />
            )}
          </GridColumn>
        </GridRow>

        <GridRow>
          {editModeActive ? (
            <Formik
              initialValues={{ displayName: profile!.displayName, bio: profile?.bio }}
              onSubmit={(values) => {
                updateProfile(values);
              }}
              validationSchema={Yup.object({
                displayName: Yup.string().required("Display name is required"),
              })}
            >
              {({ handleSubmit, isSubmitting, isValid, dirty }) => (
                <Form
                  style={{ marginLeft: 10, marginRight: 10, width: "100%" }}
                  className="ui form"
                  onSubmit={handleSubmit}
                >
                  <MyTextInput placeholder="Display Name" name="displayName" />
                  <MyTextArea placeholder="Bio" name="bio" rows={5} />
                  <Button
                    disabled={!isValid || !dirty || isSubmitting}
                    loading={isSubmitting}
                    positive
                    content="Update"
                    type="submit"
                    fluid
                  />
                </Form>
              )}
            </Formik>
          ) : (
            <GridColumn style={{ whiteSpace: "pre-line" }}>{profile?.bio}</GridColumn>
          )}
        </GridRow>
      </Grid>
    </Tab.Pane>
  );
});
