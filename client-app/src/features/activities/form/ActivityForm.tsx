import { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function ActivityForm() {
  const { activityStore } = useStore();

  const {
    selectedActivity,
    closeForm,
    createActivity,
    updateActivity,
    loading,
  } = activityStore;
  const initialState = selectedActivity ?? {
    id: "",
    title: "",
    date: "",
    description: "",
    category: "",
    city: "",
    venue: "",
  };

  const [activity, setActivity] = useState(initialState);

  function handleChange(
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = event.target;
    setActivity({ ...activity, [name]: value });
  }

  function handleSubmit() {
    activity.id ? updateActivity(activity) : createActivity(activity);
  }

  return (
    <Segment clearing>
      <Form>
        <Form.Input
          placeholder="Title"
          name="title"
          value={activity?.title}
          onChange={handleChange}
        />
        <Form.TextArea
          placeholder="Description"
          name="description"
          value={activity?.description}
          onChange={handleChange}
        />
        <Form.Input
          placeholder="Category"
          name="category"
          value={activity?.category}
          onChange={handleChange}
        />
        <Form.Input
          placeholder="Date"
          name="date"
          type="date"
          value={activity?.date}
          onChange={handleChange}
        />
        <Form.Input
          placeholder="City"
          name="city"
          value={activity?.city}
          onChange={handleChange}
        />
        <Form.Input
          placeholder="Venue"
          name="venue"
          value={activity?.venue}
          onChange={handleChange}
        />
      </Form>
      <Button
        onClick={handleSubmit}
        loading={loading}
        floated="right"
        positive
        type="submit"
        content="Submit"
      />
      <Button
        onClick={closeForm}
        floated="right"
        type="button"
        content="Cancel"
      />
    </Segment>
  );
});
