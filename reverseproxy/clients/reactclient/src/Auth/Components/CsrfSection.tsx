import { Button } from "../../Common/Components/Button";
import { HeaderOne } from "../../Common/Components/HeaderOne";
import { Paragraph } from "../../Common/Components/Paragraph";
import { getCsrfTokenData } from "../DataServices/CsrfTokenDataService";

interface ICsrfSectionProps {
    buttonsDisabled: (bool: boolean) => void;
}

function CsrfSection(props: ICsrfSectionProps): JSX.Element {
    const { buttonsDisabled } = props;

    async function getCsrfToken(): Promise<void> {
        await getCsrfTokenData();
        buttonsDisabled(false);
    }

    return (
        <>
            <HeaderOne>Get CSRF Token</HeaderOne>
            <Paragraph
                red={'red'}
                bold={'bold'}
            >
                Must retrieve Csrf Token before each button.
            </Paragraph>
            <Button onClick={() => getCsrfToken()}>Get Csrf Token</Button>
        </>
    );
}

export default CsrfSection;