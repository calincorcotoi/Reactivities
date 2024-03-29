import { useEffect, useState } from "react";
import { Button, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ActivityFormValues } from "../../../app/models/activity";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";
import MySelectInput from "../../../app/common/form/MySelectInput";
import { categoryOptions } from "../../../app/common/options/categoryOptions";
import MyDateInput from "../../../app/common/form/MyDateInput";
import { v4 as uuid } from "uuid";

export default observer(function ActivityForm() {
  const { activityStore } = useStore();

  const { loadActivity, loadingInitial } = activityStore;

  const { id } = useParams();
  const navigate = useNavigate();
  const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues());

  useEffect(() => {
    if (id) loadActivity(id).then((activity) => setActivity(new ActivityFormValues(activity)));
  }, [id, loadActivity]);

  const validationSchema = Yup.object().shape({
    title: Yup.string().required("The activity title is required"),
    description: Yup.string().required("The activity description title is required"),
    category: Yup.string().required(),
    date: Yup.string().required(),
    city: Yup.string().required(),
    venue: Yup.string().required(),
  });

  async function handleFormSubmit(activity: ActivityFormValues) {
    if (!activity.id) {
      activity.id = uuid();
      await activityStore.createActivity(activity);
      navigate(`/activities/${activity.id}`);
    } else {
      await activityStore.updateActivity(activity);
      navigate(`/activities/${activity.id}`);
    }
  }

  if (loadingInitial) return <LoadingComponent content="Loading activity..." />;

  return (
    <Segment clearing>
      <Header content="Activity Details" color="teal"></Header>
      <Formik
        enableReinitialize
        initialValues={activity}
        onSubmit={(values) => handleFormSubmit(values)}
        validationSchema={validationSchema}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <>
            <Form className="ui form" onSubmit={handleSubmit}>
              <MyTextInput placeholder="Title" name="title" />
              <MyTextArea placeholder="Description" name="description" rows={3} />
              <MySelectInput placeholder="Category" name="category" options={categoryOptions} />
              <MyDateInput
                name="date"
                placeholderText="Date"
                showTimeSelect
                timeCaption="time"
                dateFormat="MMMM d, yyyy h:mm aa"
              />
              <MyTextInput placeholder="City" name="city" />
              <MyTextInput placeholder="Venue" name="venue" />
              <Button
                loading={isSubmitting}
                floated="right"
                positive
                type="submit"
                content="Submit"
                disabled={!isValid || isSubmitting || !dirty}
              />
              <Button as={Link} to="/activities" floated="right" type="button" content="Cancel" />
            </Form>
          </>
        )}
      </Formik>
    </Segment>
  );
});
