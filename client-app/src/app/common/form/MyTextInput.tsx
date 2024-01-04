import { useField } from "formik";
import { FormField, Label } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  label?: string;
  type?: string;
}

export default function MyTextInput(props: Props) {
  const [field, meta] = useField(props.name);
  return (
    <FormField error={meta.error && meta.touched}>
      <label> {props.label} </label>
      <input {...field} {...props}></input>
      {meta.error && meta.touched && (
        <Label basic color="red">
          {meta.error}
        </Label>
      )}
    </FormField>
  );
}
