import { useField } from "formik";
import { FormField, Label, Select } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  options: any;
  label?: string;
}

export default function MySelectInput(props: Props) {
  const [field, meta, helpers] = useField(props.name);
  return (
    <FormField error={meta.error && meta.touched}>
      <label> {props.label} </label>
      <Select
        clearable
        options={props.options}
        placeholder={props.placeholder}
        value={field.value || null}
        onChange={(_, d) => helpers.setValue(d.value)}
        onBlur={() => helpers.setTouched(true)}
      />
      {meta.error && meta.touched && (
        <Label basic color="red">
          {meta.error}
        </Label>
      )}
    </FormField>
  );
}
