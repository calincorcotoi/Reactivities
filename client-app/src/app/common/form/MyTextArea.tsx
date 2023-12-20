import { useField } from "formik";
import { FormField, Label } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  rows: number;
  label?: string;
}

export default function MyTextArea(props: Props) {
  const [field, meta] = useField(props.name);
  return (
    <FormField error={meta.error && meta.touched}>
      <label> {props.label} </label>
      <textarea {...field} {...props}></textarea>
      {meta.error && meta.touched && (
        <Label basic color="red">
          {meta.error}
        </Label>
      )}
    </FormField>
  );
}
